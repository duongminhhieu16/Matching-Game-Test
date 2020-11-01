using NUnit.Framework;
using UnityEngine;
using UnityEditor;

namespace Tests
{
    public class BoardControllerTest
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
        // With CheckIfChange() method, we use Node Coverage. 
        //There are 6 nodes with edges: 1-3, 3-4, 4-5, 1-2, 3-6, 6-2. 
        // Initial node: 1. Final node: 2, 5.

        // Test 1 for method CheckIfChanged: tile1 == sprite[5]. Path: (1, 2). Expected: false. Actual: false.
        [Test]
        public void TestCheckIfChanged1()
        {
            SetUpBeforeTest();
            Vector2Int tile1 = new Vector2Int(1, 1);
            Vector2Int tile2 = new Vector2Int(2, 2);
            GameObject a = new GameObject();
            a.AddComponent<SpriteRenderer>();
            SpriteRenderer renderer1 = a.GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[5];
            boardController.board.allTiles[tile1.x, tile1.y] = a;
            Assert.False(boardController.CheckIfChanged(tile1, tile2));
        }

        // Test 2 for method CheckIfChanged: tile1 != sprite[5], tile2 != sprite[5]
        // There is no combo when swapping 2 tiles (2 tiles must be adjacent). 
        // Path: (1, 3, 6, 2). Expected: false. Actual: false.
        [Test]
        public void TestCheckIfChanged2()
        {
            SetUpBeforeTest();
            Vector2Int tile1 = new Vector2Int(0, 0);
            Vector2Int tile2 = new Vector2Int(0, 1);
            SpriteRenderer renderer1 = boardController.board.allTiles[tile1.x, tile1.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[0];

            renderer1 = boardController.board.allTiles[tile2.x, tile2.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[0];

            renderer1 = boardController.board.allTiles[tile2.x+1, tile2.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[5];

            renderer1 = boardController.board.allTiles[tile2.x+2, tile2.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[5];

            renderer1 = boardController.board.allTiles[tile1.x+1, tile1.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[5];

            renderer1 = boardController.board.allTiles[tile1.x+2, tile1.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[5];

            renderer1 = boardController.board.allTiles[tile1.x, tile1.y+2].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[5];

            Assert.False(boardController.CheckIfChanged(tile1, tile2));
        }

        // Test 3 for method CheckIfChanged: tile1 != sprite[5], tile2 != sprite[5]
        // There is a combo when swapping 2 tiles (2 tiles must be adjacent). 
        // Path: (1, 3, 4, 5). Expected: true. Actual: true.
        [Test]
        public void TestCheckIfChanged3()
        {
            SetUpBeforeTest();
            Vector2Int tile1 = new Vector2Int(0, 0);
            Vector2Int tile2 = new Vector2Int(0, 1);
            SpriteRenderer renderer1 = boardController.board.allTiles[tile1.x, tile1.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[0];

            renderer1 = boardController.board.allTiles[tile2.x, tile2.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[1];

            renderer1 = boardController.board.allTiles[tile2.x + 1, tile2.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[5];

            renderer1 = boardController.board.allTiles[tile2.x + 2, tile2.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[5];

            renderer1 = boardController.board.allTiles[tile1.x + 1, tile1.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[5];

            renderer1 = boardController.board.allTiles[tile1.x + 2, tile1.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[5];

            renderer1 = boardController.board.allTiles[tile1.x, tile1.y + 2].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[0];

            renderer1 = boardController.board.allTiles[tile1.x, tile1.y + 3].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[0];

            Assert.True(boardController.CheckIfChanged(tile1, tile2));
        }

        //********************************************************************************//
        // With CheckMatch() method, we will use Node Coverage. 
        // We have 7 nodes. Edges are: 1-2, 2-3, 3-4, 4-5, 5-6, 5-3, 6-3, 4-3, 6-2, 2-7.
        // Initial node: 1. Final node: 7.

        // Test 1: board has combo(s), so we will manually create combo at (0, 0), (0, 1), (0, 2).
        // Path will tour edges: 1-2, 2-3, 3-4, 4-5, 5-6, 6-2, 2-7. So it has all 7 nodes.
        // Expected: True. Actual: True.
        [Test]
        public void TestCheckMatch1()
        {
            SetUpBeforeTest();
            Vector2Int tile1 = new Vector2Int(0, 0);
            SpriteRenderer renderer1 = boardController.board.allTiles[tile1.x, tile1.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[0];

            renderer1 = boardController.board.allTiles[tile1.x, tile1.y+1].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[0];

            renderer1 = boardController.board.allTiles[tile1.x, tile1.y+2].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[0];

            Assert.True(boardController.CheckMatch());
        }

        //********************************************************************************//
        // With FindColumnMatchForTile() (find horizontal combo) method, we will use Node Coverage. 
        // We have 5 nodes. Edges are: 1-2, 2-3, 3-4, 4-3, 3-5, 4-5.
        // Initial node: 1. Final node: 5.

        // Test 1: There is no column match at tile (0, 0). We will manually create it by set sprite[0] at tile (0, 0), sprite[5] at tile (1, 0).
        // So it will stop when nextColumn.sprite != sprite.
        // Path will tour edges: 1-2, 2-3, 3-4, 4-5.
        // Expected: 0 (List has no element). Actual: 0.
        [Test]
        public void TestFindColumnMatchForTile1()
        {
            SetUpBeforeTest();
            Vector2Int tile1 = new Vector2Int(1, 0);
            SpriteRenderer renderer1 = boardController.board.allTiles[tile1.x, tile1.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[5];
            
            tile1 = new Vector2Int(0, 0);
            renderer1 = boardController.board.allTiles[tile1.x, tile1.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[0];

            Assert.Zero(boardController.FindColumnMatchForTile(tile1.x, tile1.y, renderer1.sprite).Count);
        }

        //********************************************************************************//
        // With FindRowMatchForTile() (find vertical combo) method, we will use Node Coverage. 
        // We have 5 nodes. Edges are: 1-2, 2-3, 3-4, 4-3, 3-5, 4-5.
        // Initial node: 1. Final node: 5.

        // Test 1: There is no column match at tile (0, 0). We will manually create it by set sprite[0] at tile (0, 0), sprite[5] at tile (0, 1).
        // So it will stop when nextRow.sprite != sprite.
        // Path will tour edges: 1-2, 2-3, 3-4, 4-5.
        // Expected: 0 (List has no element). Actual: 0.
        [Test]
        public void TestFindRowMatchForTile1()
        {
            SetUpBeforeTest();
            Vector2Int tile1 = new Vector2Int(0, 1);
            SpriteRenderer renderer1 = boardController.board.allTiles[tile1.x, tile1.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[5];

            tile1 = new Vector2Int(0, 0);
            renderer1 = boardController.board.allTiles[tile1.x, tile1.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = boardController.board.sprites[0];

            Assert.Zero(boardController.FindColumnMatchForTile(tile1.x, tile1.y, renderer1.sprite).Count);
        }
        //********************************************************************************//
        // With SwapTiles() method, we will use Node Coverage. 
        // We have 3 nodes. Edges are: 1-2, 2-3.
        // Initial node: 1. Final node: 3.

        // Test 1: Swap two tiles.
        // We mannually set sprite[0] at tile (0, 0) and sprite[1] at tile (0, 1).
        // After swap, we get sprite[1] at tile (0, 0) and sprite[0] at tile (0, 1).
        // Path will tour edges: 1-2, 2-3.
        // Expected: True. Actual: True.
        [Test]
        public void TestSwapTiles1()
        {
            SetUpBeforeTest();
            Sprite sprite0 = boardController.board.sprites[0];
            Sprite sprite1 = boardController.board.sprites[1];
            Vector2Int tile1 = new Vector2Int(0, 0);
            Vector2Int tile2 = new Vector2Int(0, 1);
            SpriteRenderer renderer1 = boardController.board.allTiles[tile1.x, tile1.y].GetComponent<SpriteRenderer>();
            renderer1.sprite = sprite0;

            SpriteRenderer renderer2 = boardController.board.allTiles[tile2.x, tile2.y].GetComponent<SpriteRenderer>();
            renderer2.sprite = sprite1;

            boardController.SwapTiles(tile1, tile2);
            Assert.True(renderer1.sprite == sprite1 && renderer2.sprite == sprite0);
        }
        //********************************************************************************//
        // With CheckOneMatchAtLeast() method, we will use Node Coverage. 
        // We have 11 nodes. Edges are: 1-2, 2-3, 2-11, 3-4, 4-5, 4-10, 5-6, 5-7, 5-4, 6-8, 6-7, 7-8, 7-9, 9-4, 10-2.
        // Initial node: 1. Final nodes: 8, 11.

        // Test 1: There is no possible move (no match can be created).
        // We manually create block at all tiles (= sprite[5]).
        // Path will tour edges: 1-2, 2-3, 3-4, 4-5, 5-6, 6-7, 5-7, 7-9, 9-4, 4-10, 2-11.
        // Expected: False. Actual: False.
        [Test]
        public void TestCheckOneMatchAtLeast1()
        {
            SpriteRenderer renderer;
            SetUpBeforeTest();
            for(int col = 0; col < boardController.board.dimension; col++)
            {
                for(int row = 0; row < boardController.board.dimension; row++)
                {
                    renderer = boardController.board.allTiles[col, row].GetComponent<SpriteRenderer>();
                    renderer.sprite = boardController.board.sprites[5];
                }
            }
            Assert.False(boardController.CheckOnePossibleMatchAtleast());
        }

        // Test 2: There is a possible move (a match can be created).
        // We manually create a possible move when swapping 2 tiles: (0, 0) and (1, 0).
        // Path will tour edges: 1-2, 2-3, 3-4, 4-5, 5-6, 6-8.
        // Expected: True. Actual: True.
        [Test]
        public void TestCheckOneMatchAtLeast2()
        {
            SpriteRenderer renderer;
            SetUpBeforeTest();
            Sprite sprite0 = boardController.board.sprites[0];
            Sprite sprite1 = boardController.board.sprites[1];
            for (int col = 0; col < boardController.board.dimension; col++)
            {
                for (int row = 0; row < boardController.board.dimension; row++)
                {
                    renderer = boardController.board.allTiles[col, row].GetComponent<SpriteRenderer>();
                    renderer.sprite = boardController.board.sprites[5];
                }
            }

            renderer = boardController.board.allTiles[0, 0].GetComponent<SpriteRenderer>();
            renderer.sprite = sprite0;

            renderer = boardController.board.allTiles[1, 0].GetComponent<SpriteRenderer>();
            renderer.sprite = sprite1;

            renderer = boardController.board.allTiles[2, 0].GetComponent<SpriteRenderer>();
            renderer.sprite = sprite0;

            renderer = boardController.board.allTiles[3, 0].GetComponent<SpriteRenderer>();
            renderer.sprite = sprite0;

            Assert.True(boardController.CheckOnePossibleMatchAtleast());
        }

        //********************************************************************************//
    }
}
