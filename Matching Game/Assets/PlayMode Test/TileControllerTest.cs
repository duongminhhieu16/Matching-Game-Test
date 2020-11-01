using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

namespace Tests
{
    public class TileControllerTest
    {
        BoardController boardController;
        public void SetUpBeforeTest()
        {
            GameObject a = new GameObject();
            a.AddComponent<BoardData>();
            a.AddComponent<BoardController>();
            a.AddComponent<TileController>();
            boardController = a.GetComponent<BoardController>();
            boardController.board = a.GetComponent<BoardData>();
            boardController.board.sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Sprites/1.png"));
            boardController.board.sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Sprites/2.png"));
            boardController.board.sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Sprites/3.png"));
            boardController.board.sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Sprites/4.png"));
            boardController.board.sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Sprites/5.png"));
            boardController.board.sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Sprites/6.png"));
            boardController.board.tilePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Tile.prefab");
            boardController.board.allTiles = new GameObject[boardController.board.dimension, boardController.board.dimension];
            boardController.SetUpBoard();
        }
        //********************************************************************************//  
        [Test]
        public void TileControllerTestSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // Test Move() method
        public IEnumerator Method(Vector3 actual, Vector3 expected)
        {
            yield return null;
            Assert.AreEqual(expected, actual);
        }
        [UnityTest]
        public IEnumerator TestMove()
        {
            SetUpBeforeTest();
            TileController tile1 = boardController.board.allTiles[0, 0].GetComponent<TileController>();
            Vector3 current_pos = boardController.board.pos[0, 1];
            
            yield return tile1.Move(boardController.board.pos[0, 0], boardController.board.pos[0, 1]);
            yield return null;
            yield return Method(current_pos, tile1.pos);
        }
    }
}
