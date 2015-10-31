using UnityEngine;
using System.Collections;

public class ButtonContainer : MonoBehaviour {

    //1,游戏数据的保存，和场景之间游戏数据的传递使用 PlayerPrefs
    //2,场景的分类
        //2.1开始场景
        //2.2角色选择界面 
        //2.3游戏玩家打怪的界面，就是游戏实际的运行场景 村庄有野兽。。。


    //开始新游戏
    public void OnNewGame() {
        PlayerPrefs.SetInt("DataFromSave", 0); 
        // 加载我们的选择角色的场景 2
    }
    //加载已经保存的游戏
    public void OnLoadGame() {
        PlayerPrefs.SetInt("DataFromSave", 1); //DataFromSave表示数据来自保存
        //加载我们的play场景3
    }
	
}
