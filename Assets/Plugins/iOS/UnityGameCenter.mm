//
//  UnityGameCenter.m
//  bearsaga
//
//  Created by nasa8f on 2017/2/2.
//  Copyright © 2017年 nasa8f. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "unityswift-Swift.h"

extern "C" {
    void GameCenterSave(const char *message) {
        // You can access Swift classes directly here.
        [UnityGameCenter Save:[NSString stringWithUTF8String:message]];
    }
    
    void GameCenterLoad() {
        [UnityGameCenter Load];
    }
    
    void GameCenterDelete() {
        [UnityGameCenter Delete];
    }
}
