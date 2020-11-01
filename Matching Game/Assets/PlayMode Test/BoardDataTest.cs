using NUnit.Framework;
using UnityEngine;
using UnityEditor;

namespace Tests
{
    public class NewTestScript
    {
        BoardController boardController;
        public void SetUpBeforeTest()
        {
            GameObject a = new GameObject();
            a.AddComponent<BoardData>();
            a.AddComponent<BoardController>();
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
        // With GetSpriteAt() method, we will use Node Coverage. 
        // We have 4 nodes. Edges are: 1-2, 1-3, 3-4.
        // Initial node: 1. Final node: 2, 4.

        // Test 1: Invalid tile position.
        // Input: col = -1.
        // Expected: Null. Actual: Null.
        [Test]
        public void TestGetSpriteAt1()
        {
            SetUpBeforeTest();
            Assert.Null(boardController.board.GetSpriteAt(-1, 0));
        }

        // Test 2: Valid tile position.
        // Input: col = 0, row = 0.
        // Expected: Not null. Actual: Not null.
        [Test]
        public void TestGetSpriteAt2()
        {
            SetUpBeforeTest();
            Assert.NotNull(boardController.board.GetSpriteAt(0, 0));
        }

        //********************************************************************************//
        // With GetSpriteRendererAt() method, we will use Node Coverage. 
        // We have 4 nodes. Edges are: 1-2, 1-3, 3-4.
        // Initial node: 1. Final node: 2, 4.

        // Test 1: Invalid tile position.
        // Input: col = -1.
        // Expected: Null. Actual: Null.
        [Test]
        public void TestGetSpriteRendererAt1()
        {
            SetUpBeforeTest();
            Assert.Null(boardController.board.GetSpriteRendererAt(-1, 0));
        }

        // Test 2: Valid tile position.
        // Input: col = 0, row = 0.
        // Expected: Not null. Actual: Not null.
        [Test]
        public void TestGetSpriteRendererAt2()
        {
            SetUpBeforeTest();
            Assert.NotNull(boardController.board.GetSpriteRendererAt(0, 0));
        }
    }
}
