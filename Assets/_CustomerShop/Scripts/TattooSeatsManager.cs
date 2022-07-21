using System.Collections.Generic;
using Singleton;
using UnityEngine;

public class TattooSeatsManager : Singleton<TattooSeatsManager>
{
    [SerializeField] private List<GameObject> tattooSeats;

    public GameObject GetTattooSeat(int id)
    {
        return tattooSeats[id];
    }
}
