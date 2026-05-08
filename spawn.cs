using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;

public class spawn : MonoBehaviour
{
    // Create public class to spawn obj
    public GameObject circle;

    // Create public class of front
    public GameObject front;

    // Random class
    Random rnd = new Random();

    // Initialize array of spawned balls
    private GameObject[] circle_arr;

    // Public variable of number of balls
    public int num_balls;

    // Public variable of teaching mode
    public bool teaching_mode;

    // Set pos vars and bool
    private int x_pos;
    private int y_pos;

    // Start is called before the first frame update
    void Start()
    {
        // Set dictionary of possible types of balls
        Dictionary<int, Tuple<int, Color32, int>> types = new Dictionary<int, Tuple<int, Color32, int>>();
        types.Add(0, Tuple.Create(60, new Color32(255, 94, 94, 255), 1));
        types.Add(1, Tuple.Create(70, new Color32(255, 197, 94, 255), 2));
        types.Add(2, Tuple.Create(80, new Color32(255, 255, 94, 255), 3));
        types.Add(3, Tuple.Create(90, new Color32(94, 255, 110, 255), 4));
        types.Add(4, Tuple.Create(100, new Color32(138, 255, 244, 255), 5));

        // ----------------------------
        // TEACHING MODE (minimal)
        // ----------------------------
        if (teaching_mode)
        {
            int offset = 400;

            // Pick ONE random type → both balls use it
            int key = rnd.Next(0, 5);

            int size = types[key].Item1;
            Color32 color = types[key].Item2;
            int mass = types[key].Item3;

            // Random center position
            int randX = rnd.Next(-200, 201);
            int randY = rnd.Next(-200, 201);

            // Random speed (but same magnitude)
            int speed1 = rnd.Next(60, 126);
            int speed2 = rnd.Next(-126, -61);

            // Spawn left ball
            GameObject b1 = Instantiate(circle, new Vector3(randX - offset, randY), Quaternion.identity);
            Rigidbody rb1 = b1.GetComponent<Rigidbody>();
            rb1.velocity = new Vector3(speed1, 0, 0);

            // Spawn right ball
            GameObject b2 = Instantiate(circle, new Vector3(randX + offset, randY), Quaternion.identity);
            Rigidbody rb2 = b2.GetComponent<Rigidbody>();
            rb2.velocity = new Vector3(speed2, 0, 0);

            // Apply SAME struct properties to both
            foreach (GameObject b in new GameObject[] { b1, b2 })
            {
                b.transform.localScale = new Vector2(size, size);
                b.GetComponent<Rigidbody>().mass = mass;
                b.GetComponent<Renderer>().material.color = color;
            }

            return; // skip normal spawning
        }

        // ----------------------------
        // NORMAL MODE (unchanged)
        // ----------------------------
        for (int i = 0; i < num_balls; i++)
        {
            bool is_valid = false;

            while (is_valid == false)
            {
                int margin = 30;

                x_pos = rnd.Next(-1 * (int)front.transform.localScale.x / 2 + types[4].Item1 / 2 + margin, (int)front.transform.localScale.x / 2 + 1 - types[4].Item1 / 2 - margin);
                y_pos = rnd.Next(-1 * (int)front.transform.localScale.y / 2 + types[4].Item1 / 2 + margin, (int)front.transform.localScale.y / 2 + 1 - types[4].Item1 / 2 - margin);

                if (circle_arr == null)
                {
                    is_valid = true;
                }
                else
                {
                    int is_true = 1;

                    foreach (GameObject obj in circle_arr)
                    {
                        int x_other_obj = (int)obj.transform.position.x;
                        int y_other_obj = (int)obj.transform.position.y;

                        if (Math.Abs(x_pos - x_other_obj) <= types[4].Item1 / 2 + obj.transform.localScale.x / 2 + margin &&
                            Math.Abs(y_pos - y_other_obj) <= types[4].Item1 / 2 + obj.transform.localScale.y / 2 + margin)
                        {
                            is_true = 0;
                        }
                    }

                    if (is_true == 1)
                    {
                        is_valid = true;
                    }
                }
            }

            spawn_ball(x_pos, y_pos, types);
            circle_arr = GameObject.FindGameObjectsWithTag("circle");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Spawn ball method
    void spawn_ball(int x, int y, Dictionary<int, Tuple<int, Color32, int>> dict)
    {
        // Set random type of ball
        int key = rnd.Next(0, 5);

        // Set random velocity of circle
        int vel_x = rnd.Next(-100, 101);
        int vel_y = rnd.Next(-100, 101);

        // Add circle to scene as ball and create rigidbody
        GameObject ball = Instantiate(circle, new Vector3(x, y), Quaternion.identity);
        ball.GetComponent<Rigidbody>().velocity = new Vector3(vel_x, vel_y);

        // Set color, size, and mass
        ball.GetComponent<Renderer>().material.color = dict[key].Item2;
        ball.transform.localScale = new Vector2(dict[key].Item1, dict[key].Item1);
        ball.GetComponent<Rigidbody>().mass = dict[key].Item3;
    }
}
