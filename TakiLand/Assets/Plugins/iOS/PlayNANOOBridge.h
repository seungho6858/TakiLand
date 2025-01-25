//
//  PlayNANOOBridge.h
//  PlayNANOOUnity
//
//  Created by LimJongHyun on 2017. 6. 1..
//  Copyright © 2017년 LimJongHyun. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <AdSupport/AdSupport.h>
#import <UIKit/UIKit.h>
#import <PlayNANOOPlugin/PlayNANOOPlugin.h>

@interface PlayNANOOBridge : NSObject
 {
     UIViewController *viewController;
     PlayNANOOBridge *bridge;
     PNPlugin *plugin;
 }

-(id)init:(NSString *)gameID serviceKey:(NSString *)serviceKey secretKey:(NSString *)secretKey;
-(void)_pnInit;
-(void)_pnOpenView:(NSString *)url;
-(void)_pnHelpDeskClearOptional;
-(void)_pnHelpDeskOptional:(NSString *)key value:(NSString *)value;
-(void)_pnOpenHelpDeskGuestMode;
-(void)_pnOpenHelpDeskPlayerMode:(NSString *)url;
-(void)_pnOpenShare:(NSString *)title message:(NSString *)message;
@end


