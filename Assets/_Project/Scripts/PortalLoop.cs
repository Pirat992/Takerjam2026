using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PortalLoop : MonoBehaviour
    {
        [SerializeField] private PortalRoom room;
        [SerializeField] private Portal output;

        private void Start()
        {
            room.SetPortal(output);
        }
    }
}