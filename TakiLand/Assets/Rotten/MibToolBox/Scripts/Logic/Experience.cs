using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Mib
{
	[Serializable]
	public class Experience
	{
		public event Func<Experience, bool, UniTask> OnExpChanged;

		private List<UniTask> _tasks;
		private int[] _expTable;

		private int _extraExp;

		private bool _isWorking;

		[SerializeField]
		public int TotalExp
		{
			get;
			private set;
		}

		[SerializeField]
		public int Level
		{
			get;
			private set;
		} = 1;

		private int Floor => Level == 1 ? 0 : _expTable[Level - 2];
		public float Ratio => (float)CurrentExp / CurrentMax;
		public int CurrentExp => TotalExp - Floor;
		public int CurrentMax => _expTable[Level - 1] - Floor;
		public bool IsMaxLevel => Level >= _expTable.Length;

		public override string ToString() =>
			$"Total [{TotalExp}]\tExtra[{_extraExp}]\tLevel:{Level}\tCurrent[{CurrentExp}/{CurrentMax}][{Ratio}]";

		public void SetTable(int[] expTable)
		{
			_expTable = expTable;
			_tasks = new();
		}

		public async UniTask Add(int value)
		{
			if (_expTable == null)
			{
				throw new Exception("SetTable Before Add()");
			}

			if (_isWorking)
			{
				_extraExp += value;
				return;
			}

			_isWorking = true;
			int totalExp = TotalExp + value;
			while (totalExp - Floor >= CurrentMax && !IsMaxLevel)
			{
				++Level;
				TotalExp = Floor;

				await InvokeEvent(true);
			}

			TotalExp = totalExp;
			await InvokeEvent(false);
			_isWorking = false;

			if (_extraExp > 0)
			{
				int extraExp = _extraExp;
				_extraExp = 0;
				await Add(extraExp);
			}
		}

		private async UniTask InvokeEvent(bool levelUp)
		{
			if (OnExpChanged == null)
			{
				return;
			}

			_tasks.Clear();
			foreach (Delegate func in OnExpChanged.GetInvocationList())
			{
				_tasks.Add(((Func<Experience, bool, UniTask>)func)
					.Invoke(this, levelUp));
			}

			await UniTask.WhenAll(_tasks);
		}
	}
}