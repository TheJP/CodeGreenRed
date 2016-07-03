using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CardPool : MonoBehaviour
{
    public GameObject turnLeftPrefab;
    public GameObject turnRightPrefab;
    public GameObject[] commonCardsPrefabs;
    public GameObject[] rareCardsPrefab;
    public int cardsInBooster = 5;
    public int nRounds = 4;

    /// <summary>
    /// creates several boosterpacks with
    /// 2Basics and 1Common
    /// + 1B 1C or
    /// + 1B 1R or
    /// + 2C    or
    /// + 1C 1R 
    /// </summary>
    /// <returns>list of shuffled booterpacks</returns>
    public List<BoosterPack> FillBoosterBox()
    {
        List<BoosterPack> boosterPacks = new List<BoosterPack>();
        System.Random rand = new System.Random();
        for (int i = 0; i < nRounds; i++)
        {
            List<GameObject> cards = new List<GameObject>(cardsInBooster);

            //always two basic movement options
            cards.Add(rand.Next(0, 2) > 0 ? turnLeftPrefab : turnRightPrefab);
            cards.Add(rand.Next(0, 2) > 0 ? turnLeftPrefab : turnRightPrefab);
            //one common card
            cards.Add(commonCardsPrefabs[rand.Next(0, commonCardsPrefabs.Length)]);
            //either a basic or a common card
            if (rand.Next(0, 2) > 0) { cards.Add(rand.Next(0, 1) > 0 ? turnLeftPrefab : turnRightPrefab); }
            else { cards.Add(commonCardsPrefabs[rand.Next(0, commonCardsPrefabs.Length)]); }
            //either a common or a rare card
            if (rand.Next(0, 3) > 1) { cards.Add(rareCardsPrefab[rand.Next(0, rareCardsPrefab.Length)]); }
            else { cards.Add(commonCardsPrefabs[rand.Next(0, commonCardsPrefabs.Length)]); }

            cards.Shuffle();
            boosterPacks.Add(new BoosterPack(cards));
        }

        return boosterPacks;
    }
    /// <summary>
    /// for debugging; just left and right cards
    /// </summary>
    /// <returns></returns>
    public List<BoosterPack> BasicBoosterBox()
    {
        List<BoosterPack> boosterPacks = new List<BoosterPack>();
        System.Random rand = new System.Random();
        for (int i = 0; i < nRounds; i++)
        {
            List<GameObject> cards = new List<GameObject>(cardsInBooster);
            for (int j = 0; j < cardsInBooster; j++)
            {
                cards.Add(rand.Next(0, 2) > 0 ? turnLeftPrefab : turnRightPrefab);
            }
            cards.Shuffle();
            boosterPacks.Add(new BoosterPack(cards));
        }
        return boosterPacks;
    }
}

/// <summary>
/// randomly reorders a list
/// http://stackoverflow.com/questions/273313/randomize-a-listt
/// </summary>
public static class ShuffleListExtension
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}