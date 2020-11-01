using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DataManagementTest
    {
        FirebaseInit firebase;
        FacebookController facebook;
        GoogleController google;

        // Test for Firebase
        public void SetUpBeforeTest()
        {
            GameObject a = new GameObject();
            a.AddComponent<FirebaseInit>();
            a.AddComponent<FacebookController>();
            a.AddComponent<GoogleController>();
            firebase = a.GetComponent<FirebaseInit>();
            facebook = a.GetComponent<FacebookController>();
            google = a.GetComponent<GoogleController>();
        }
        [Test]
        public void TestInit()
        {
            SetUpBeforeTest();
            Assert.NotNull(firebase);
            Assert.NotNull(google);
            Assert.NotNull(facebook);
        }
        [Test]
        public async void TestCreateGuestAccount()
        {
            SetUpBeforeTest();
            await firebase.CreateGuestPlayer();
            Assert.NotNull(FirebaseInit.playerInfo);
        }
        [Test]
        public void TestCreatePlayerInfo()
        {
            SetUpBeforeTest();
            FirebaseInit.CreatePlayerInfo("a", "a", "a", "a");
            User user = new User("a", "a", "a", 0, "a", 1, 1, 0, ScoreData.startingMoves);
            Assert.AreEqual(user.userName, FirebaseInit.playerInfo.userName);
        }
        [Test]
        public async void TestLoadCurrentPlayerInfo()
        {
            SetUpBeforeTest();
            await FirebaseInit.LoadDataOfCurrentPlayer();
            Assert.NotNull(FirebaseInit.playerInfo.userName);
        }
        [Test]
        public async void TestLoadHighestPlayerScore()
        {
            SetUpBeforeTest();
            await FirebaseInit.LoadHighestScoreUsersInfo(5);
            Assert.AreEqual(5, FirebaseInit.users.Count);
        }
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
