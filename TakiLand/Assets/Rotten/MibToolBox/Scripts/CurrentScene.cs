using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mib
{
	public class CurrentScene : MonoSingleton<CurrentScene>
	{
		public Camera MainCamera
		{
			get;
			private set;
		}

		protected override bool IsPersistent => true;

		protected override void OnAwake()
		{
			SceneManager.activeSceneChanged += (prev, current) =>
			{
				Logg.Info($"SceneChanged [{prev.name}] -> [{current.name}]");
				Instance.MainCamera = Camera.main;
			};
		}
	}
}