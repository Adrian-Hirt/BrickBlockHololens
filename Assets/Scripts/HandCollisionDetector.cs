using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollisionDetector : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (PalmUpHandMenu.instance.gameMode != PalmUpHandMenu.GameMode.ColliderMode)
        {
            return;
        }
        if (other.gameObject.name.Contains("Index"))
        {
            if (other.gameObject.name.Contains("Right"))
            {
                GameObject finger = other.gameObject;
                Vector3 p_finger_world = finger.transform.position;
                Vector3 p_ge = gameObject.transform.position;
                Vector3 hit_direction = p_finger_world - p_ge;
                List<Vector3> directions = new List<Vector3>();
                directions.Add(gameObject.transform.up);
                directions.Add(-gameObject.transform.up);
                directions.Add(gameObject.transform.right);
                directions.Add(-gameObject.transform.right);
                directions.Add(gameObject.transform.forward);
                directions.Add(-gameObject.transform.forward);

                Vector3 max_direction = new Vector3(0, 0, 0);
                float max_scalar_product = float.MinValue;
                foreach (Vector3 direction in directions)
                {
                    float scalar_prod = Vector3.Dot(direction, Vector3.Normalize(hit_direction));

                    if (scalar_prod > max_scalar_product)
                    {
                        max_direction = direction;
                        max_scalar_product = scalar_prod;
                    }
                }
                Vector3 new_active = p_ge + max_direction;
                Vector3 ge_pos = gameObject.transform.localPosition;
                int x = (int)System.Math.Round(ge_pos.x, 0);
                int y = (int)System.Math.Round(ge_pos.y, 0);
                int z = (int)System.Math.Round(ge_pos.z, 0);
                GridElement ge = null;
                if (max_direction == gameObject.transform.up)
                {
                    ge = LevelGenerator.instance.GetGridElement(x, y + 1, z);
                }
                else if (max_direction == -gameObject.transform.up)
                {
                    ge = LevelGenerator.instance.GetGridElement(x, y - 1, z);
                }
                else if (max_direction == gameObject.transform.right)
                {
                    ge = LevelGenerator.instance.GetGridElement(x + 1, y, z);
                }
                else if (max_direction == -gameObject.transform.right)
                {
                    ge = LevelGenerator.instance.GetGridElement(x - 1, y, z);
                }
                else if (max_direction == gameObject.transform.forward)
                {
                    ge = LevelGenerator.instance.GetGridElement(x, y, z + 1);
                }
                else if (max_direction == -gameObject.transform.forward)
                {
                    ge = LevelGenerator.instance.GetGridElement(x, y, z - 1);
                }

                ge?.SetTapEnabled();
            }
            else
            {
                gameObject.GetComponent<GridElement>().SetDisabled();
            }
        }
    }
}
