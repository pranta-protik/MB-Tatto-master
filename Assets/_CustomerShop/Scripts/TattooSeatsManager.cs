using System.Collections.Generic;
using Singleton;
using UnityEngine;

public class TattooSeatsManager : Singleton<TattooSeatsManager>
{
    [SerializeField] private List<GameObject> tattooSeats;
    [SerializeField] private List<GameObject> tattooArtists;

    public GameObject GetTattooSeat(int id)
    {
        return tattooSeats[id];
    }
    
    public GameObject GetRandomTattooArtist()
    {
        int randomId = Random.Range(0, tattooArtists.Count);
        return tattooArtists[randomId];
    }
}
