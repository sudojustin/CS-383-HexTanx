using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class Tests
{
    
    public void LoadTestScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

   
    [UnityTest]
    public IEnumerator TestsWithEnumeratorPasses()
    {
        //var Tilemanager = GameObject.FindObjectOfType<TileManager>();
        for(int i = 1; i < 100; ++i)
        {
            for(int t = 1; t < 100; ++t)
            {
                //Tilemanager.MakeMap(i,t);
            }
        }
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
