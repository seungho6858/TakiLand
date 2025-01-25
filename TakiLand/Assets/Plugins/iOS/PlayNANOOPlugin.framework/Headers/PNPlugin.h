//
//  PNPlugin.h
//  PlayNANOOPlugin_BaseTool
//
//  Created by JONGHYUN LIM on 14/08/2019.
//  Copyright © 2019 JONGHYUN LIM. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <PlayNANOOPlugin/PNDelegate.h>

NS_ASSUME_NONNULL_BEGIN

NSString* _gameId;
NSString* _serviceKey;
NSString* _secretKey;

@interface PNPlugin : NSObject {
    NSMutableDictionary *optionalDict;
}
//@property (nonatomic, strong) id<PNDelegate> delegate;
@property (nonatomic, strong) id<PNUnitySendMessageDelegate> unitySendMessageDelegate;

-(instancetype)initWithPlugin:(NSString *)gameId serviceKey:(NSString *)serviceKey secretKey:(NSString *)secretKey;
-(void)openView:(UIViewController *)viewController url:(NSString *)url webViewDelegate:(id<PNDelegate>)webViewDelegate;
-(void)helpDeskOptional:(NSString *)key value:(NSString *)value;
-(void)helpDeskClearOptional;
-(void)openHelpDeskGuestMode:(UIViewController *)viewController webViewDelegate:(id<PNDelegate>)webViewDelegate;
-(void)openHelpDeskPlayerMode:(UIViewController *)viewController userId:(NSString *)userId webViewDelegate:(id<PNDelegate>)webViewDelegate;
-(void)openShare:(UIViewController *)viewController title:(NSString *)title message:(NSString *)message;
@end

NS_ASSUME_NONNULL_END
