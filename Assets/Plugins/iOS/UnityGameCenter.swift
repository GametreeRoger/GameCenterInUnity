//
//  UnityGameCenter.swift
//  GameCenterInUnity
//
//  Created by nasa8f on 2017/2/2.
//  Copyright © 2017年 Roger. All rights reserved.
//

import Foundation
import GameKit

class UnityGameCenter: NSObject {
    enum eErrorCode: String {
        case DeleteSucceed = "3"
        case LoadSucceed = "2"
        case SaveSucceed = "1"
        case NoAuth = "-1"
        case SaveError = "-2"
        case FetchError = "-3"
        case NoData = "-4"
        case LoadError = "-5"
        case EncodeError = "-6"
        case DecodeError = "-7"
        case DeleteError = "-8"
    }
    
    static let ErrorMessage = "OnErrorMessage"
    static let LoadData = "OnLoadData"
    static let UnityGameObject = "GameCenterManager"
    
    static func Save(_ saveData: String) {
        guard let data = saveData.data(using: String.Encoding.utf8) else {
            self.UnityCallback(ErrorMessage, eErrorCode.EncodeError.rawValue)
            return
        }
        
        let localPlayer = GKLocalPlayer.localPlayer()
        if !GKLocalPlayer.localPlayer().isAuthenticated {
            self.UnityCallback(ErrorMessage, eErrorCode.NoAuth.rawValue)
            return
        }
        
        localPlayer.saveGameData(data, withName: "SaveGame"){
            (saveGame: GKSavedGame?, error: Error?) -> Void in
            if error != nil {
                print("Error saving: \(error)")
                self.UnityCallback(ErrorMessage, eErrorCode.SaveError.rawValue)
            } else {
                print("Save game success!")
                self.UnityCallback(ErrorMessage, eErrorCode.SaveSucceed.rawValue)
            }
        }
    }
    
    static func Load() {
        let localPlayer = GKLocalPlayer.localPlayer()
        if !GKLocalPlayer.localPlayer().isAuthenticated {
            self.UnityCallback(ErrorMessage, eErrorCode.NoAuth.rawValue)
            return
        }
        
        localPlayer.fetchSavedGames() {
            (saveGames: [GKSavedGame]?, error: Error?) -> Void in
            if error != nil {
                print("fetch saving error: \(error)")
                self.UnityCallback(ErrorMessage, eErrorCode.FetchError.rawValue)
            } else {
                print("game save lenth: \(saveGames?.count)")
                guard (saveGames?.count)! > 0 else {
                    self.UnityCallback(ErrorMessage, eErrorCode.NoData.rawValue)
                    return
                }
                let save = saveGames?.first
                save?.loadData() {
                    (data: Data?, error: Error?) -> Void in
                    if error != nil {
                        print("Error load data: \(error)")
                        self.UnityCallback(ErrorMessage, eErrorCode.LoadError.rawValue)
                        return
                    } else {
                        guard let dataString = String(data: data!, encoding: .utf8) else {
                            self.UnityCallback(ErrorMessage, eErrorCode.DecodeError.rawValue)
                            return
                        }
                        
                        self.UnityCallback(LoadData, dataString)
                    }
                }
            }
        }
    }
    
    static func Delete() {
        let localPlayer = GKLocalPlayer.localPlayer()
        if !GKLocalPlayer.localPlayer().isAuthenticated {
            self.UnityCallback(ErrorMessage, eErrorCode.NoAuth.rawValue)
            return
        }
        
        localPlayer.deleteSavedGames(withName: "SaveGame") {
            (error: Error?) -> Void in
            if error != nil {
                print("Can not delete save game.")
                self.UnityCallback(ErrorMessage, eErrorCode.DeleteError.rawValue)
            } else {
                print("Delete save game succeed.")
                self.UnityCallback(ErrorMessage, eErrorCode.DeleteSucceed.rawValue)
            }
        }
    }
    
    static func UnityCallback(_ msgKind: String, _ message: String) {
        UnitySendMessage(UnityGameObject, msgKind, message)
    }
}
