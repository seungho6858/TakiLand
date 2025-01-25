//
//  PlayNANOOBridge.m
//  PlayNANOOUnity
//
//  Created by LimJongHyun on 2017. 6. 1..
//  Copyright © 2017년 LimJongHyun. All rights reserved.
//

#import "PlayNANOOBridge.h"

extern "C" UIViewController *UnityGetGLViewController();
extern "C" void UnitySendMessage(const char *, const char *, const char *);

@implementation PlayNANOOBridge

-(id)init:(NSString *)gameID serviceKey:(NSString *)serviceKey secretKey:(NSString *)secretKey {
    self = [super init];
    viewController = UnityGetGLViewController();
    plugin = [[PNPlugin alloc] initWithPlugin:gameID serviceKey:serviceKey secretKey:secretKey];

    return self;
}

-(void)_pnInit{}

-(void)_pnOpenView:(NSString *)url {
    [plugin openView:viewController url:url webViewDelegate:(id)self];
}

-(void)_pnHelpDeskClearOptional {
    [plugin helpDeskClearOptional];
}

-(void)_pnHelpDeskOptional:(NSString *)key value:(NSString *)value {
    [plugin helpDeskOptional:key value:value];
}

-(void)_pnOpenHelpDeskGuestMode {
    [plugin openHelpDeskGuestMode:viewController webViewDelegate:(id)self];
}

-(void)_pnOpenHelpDeskPlayerMode:(NSString *)userId {
    [plugin openHelpDeskPlayerMode:viewController userId:userId webViewDelegate:(id)self];
}

-(void)_pnOpenShare:(NSString *)title message:(NSString *)message {
    [plugin openShare:viewController title:title message:message];
}

-(void)unitySendMessage:(NSString *)functionName message:(NSString *)message {
    const char *className = [@"PlayNANOO" UTF8String];
    const char *methodName = [functionName UTF8String];
    const char *value = [message UTF8String];
    UnitySendMessage(className, methodName, value);   
}
@end

extern "C" {
    PlayNANOOBridge *bridge;

    void _pnInit(const char* gameID, const char* serviceKey, const char* secretKey, const char* version) {
        bridge = [[PlayNANOOBridge alloc] init:[NSString stringWithUTF8String:gameID] serviceKey:[NSString stringWithUTF8String:serviceKey] secretKey:[NSString stringWithUTF8String:secretKey]];
    }

    void _pnHelpDeskClearOptional() {
        [bridge _pnHelpDeskClearOptional];
    }

    void _pnHelpDeskOptional(const char* key, const char* value) {
        [bridge _pnHelpDeskOptional:[NSString stringWithUTF8String:key] value:[NSString stringWithUTF8String:value]];
    }

    void _pnOpenView(const char* url) {
        [bridge _pnOpenView:[NSString stringWithUTF8String:url]];
    }

    void _pnOpenHelpDeskGuestMode() {
        [bridge _pnOpenHelpDeskGuestMode];
    }

    void _pnOpenHelpDeskPlayerMode(const char* userId) {
        [bridge _pnOpenHelpDeskPlayerMode:[NSString stringWithUTF8String:userId]];
    }

    void _pnOpenShare(const char* title, const char* message) {
        [bridge _pnOpenShare:[NSString stringWithUTF8String:title] message:[NSString stringWithUTF8String:message]];
    }
}
