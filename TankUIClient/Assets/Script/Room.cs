using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfMembers { get; set; }

        public Room(int _id, string _name, int _numberOfMembers)
        {
            Id = _id;
            Name = _name;
            NumberOfMembers = _numberOfMembers;
        }

        public void Print()
        {
            Debug.Log($"{Id}: Room {Name} has {NumberOfMembers} members.");
        }
}

