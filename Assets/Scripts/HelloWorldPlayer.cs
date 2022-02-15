
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>(); //Poner una lista en vez de vector3
        Renderer rend;
        List<Color> colorList = new List<Color>() { //Lista que otorga diferentes colores

            Color.black,
            Color.blue,
            Color.cyan,
            Color.green,
            Color.red,
            Color.yellow,
            Color.white,
            Color.magenta,
            Color.gray,
            new Color(255F, 0F, 255F),
            new Color(0F, 255F, 255F),
            new Color(255F, 255F, 0F),
            new Color(128F, 0F, 128F),
            new Color(128F, 0F, 0F),
            new Color(128F, 128, 0F),
            new Color(0F, 0F, 0F),
            new Color(128F, 255F, 0F),
            new Color(0F, 255F, 128F)

        };


        public override void OnNetworkSpawn() //Cuando el player nace
        {
             if (IsOwner)
            {
            Move(); 
            rend = GetComponent<Renderer>();   

            rend.material.color = colorList [Random.Range(0, colorList.Count)]; //Hace que el color sea aleatorio
            }
        }

        public void Move() 
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc] //funcion en el servidor que se encarge de guardar los colores 1 o 2 funciones con una lista
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane(); //guarda en la variable en rede un valor aleatorio
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            transform.position = Position.Value;
        }
    }
}