using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class KatanaControler : MonoBehaviour
{
    //　変数宣言部 //
    public GameObject Katana; // 刀オブジェクトを指定するための変数
    private GameObject _child; // Unityちゃんの右手の子オブジェクトを格納するための変数
    private Quaternion _childRotation; // Unityちゃんの右手の子オブジェクトの角度を格納するための変数
    private GameObject _parent; // Unityちゃんの右手のオブジェクトを格納するための変数
    
    // ゲーム実行時に実行される関数 //
    void Start()
    {
        _child = transform.FindDeep("Katana", true).gameObject; // 刀オブジェクトを検索して格納
        Quaternion rotation = _child.transform.localRotation; // 刀オブジェクトの相対角度を変数に格納
        _childRotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w); // 角度の値を変数に値渡し(参照渡しだとダメ)
        _parent = _child.transform.parent.gameObject; // 刀オブジェクトの親オブジェクト(Unityちゃんの右手)を変数に格納
    }

    // 毎フレーム実行される関数 //
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) // キーボードの "H" が押された時
        {
            if (_child == null) // Unityちゃんが刀を持っていない時
            {
                _child = Instantiate(Katana, _parent.transform, true); // 刀を複製
                
                // 右手を重なってしまうので、刀の位置を右手との相対位置で修正 //
                Vector3 position = new Vector3(0, 0, 0.03f);
                _child.transform.localPosition = position; 
                
                _child.transform.localRotation = _childRotation; // 起動時の刀の角度と同じ角度の情報を刀オブジェクトに与える
                _child.GetComponent<Rigidbody>().useGravity = true; // 刀の重力処理を適用する
                _child.GetComponent<Rigidbody>().isKinematic = true; // 手に追従するようにする(運動力学を無視)
            }
            else // Unityちゃんが刀を持っている時
            {
                    _child.transform.parent = null; // 右手の子オブジェクトをやめる(右手から放たれる)
                    _child.GetComponent<Rigidbody>().isKinematic = false; // 運動力学を適用する
                    _child = null; // 「Unityちゃんが持っているオブジェクトはない」という意味で null を格納
            }
        }
    }
}

// 以下は、オブジェクトを名前で検索するための処理 ( http://baba-s.hatenablog.com/entry/2014/08/01/101104 を参考にした) //

public static class GameObjectExtensions
{
    /// <summary>
    /// 深い階層まで子オブジェクトを名前で検索して GameObject 型で取得します
    /// </summary>
    /// <param name="self">GameObject 型のインスタンス</param>
    /// <param name="name">検索するオブジェクトの名前</param>
    /// <param name="includeInactive">非アクティブなオブジェクトも検索する場合 true</param>
    /// <returns>子オブジェクト</returns>
    public static GameObject FindDeep( 
        this GameObject self, 
        string name, 
        bool includeInactive = false )
    {
        var children = self.GetComponentsInChildren<Transform>( includeInactive );
        foreach ( var transform in children )
        {
            if ( transform.name == name )
            {
                return transform.gameObject;
            }
        }
        return null;
    }
}
    
public static class ComponentExtensions
{
    /// <summary>
    /// 深い階層まで子オブジェクトを名前で検索して GameObject 型で取得します
    /// </summary>
    /// <param name="self">GameObject 型のインスタンス</param>
    /// <param name="name">検索するオブジェクトの名前</param>
    /// <param name="includeInactive">非アクティブなオブジェクトも検索する場合 true</param>
    /// <returns>子オブジェクト</returns>
    public static GameObject FindDeep( 
        this Component self, 
        string name, bool 
        includeInactive = false )
    {
        var children = self.GetComponentsInChildren<Transform>( includeInactive );
        foreach ( var transform in children )
        {
            if ( transform.name == name )
            {
                return transform.gameObject;
            }
        }
        return null;
    }
}
