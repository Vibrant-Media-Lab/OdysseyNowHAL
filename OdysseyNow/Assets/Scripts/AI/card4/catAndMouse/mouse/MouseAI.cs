using InControl;
using UnityEngine;
using UnityEngine.AI;
using System;
using Random = System.Random;

public class MouseAI : MonoBehaviour {
    private NavMeshAgent agent;

    private int?[,] q_mat = new int?[23716, 5];

    private int[,] grid = {
        {1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0}, {0, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1},
        {1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1}, {1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 1, 1, 1, 1},
        {1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1}, {1, 1, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 1, 1},
        {1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1}, {1, 1, 1, 1, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1}, {1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1},
        {0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0}
    };

    private float lr = 1f;
    private float dr = 0.9f;
    private float e = 1f;

    public Transform target;
    public int level;

    // Start is called before the first frame update
    private void Start() {
        // Agent tend to rotate game object, that can be
        //      undesirable
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        CreateInitialMatrix(q_mat);
    }

    // Update is called once per frame
    private void Update() {
        if (!agent.enabled) {
            return;
        }

        if ((transform.position - target.position).magnitude < 1) {
            Debug.Log("Mouse has the cheese");
        }

        switch (level) {
            case 1:
                Mouse();
                //agent.SetDestination(target.position);
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }

    // disgusting ugly gross horrible dirty
    private void CreateInitialMatrix(int?[,] q) {
        var state = 0;
        // mouse y
        for (var mr = 0; mr < 11; mr++) {
            // mouse x
            for (var mc = 0; mc < 14; mc++) {
                // cat y
                for (var cr = 0; cr < 11; cr++) {
                    // cat x
                    for (var cc = 0; cc < 14; cc++) {
                        // if the mouse is in the mouse hole
                        // if (mr == 10 && mc == 13) {
                        //     continue;
                        // }

                        // if the mouse or cat is on a wall, it is an invalid state
                        if (grid[mr, mc] == 0 || grid[cr, cc] == 0) {
                            q[state, 0] = null;
                            q[state, 1] = null;
                            q[state, 2] = null;
                            q[state, 3] = null;
                            q[state++, 4] = null;
                            continue;
                        }

                        // if the cat is on the mouse, it only counts as a single state
                        if (mr == cr && mc == cc) {
                            q[state, 0] = null;
                            q[state, 1] = null;
                            q[state, 2] = null;
                            q[state, 3] = null;
                            q[state++, 4] = null;
                            continue;
                        }

                        // mouse x
                        switch (mc) {
                            case 0:
                                q[state, 3] = null; // east
                                q[state, 4] = grid[mr, mc + 1] == 1 ? 0 : (int?) null; // west
                                break;
                            case 13:
                                q[state, 3] = grid[mr, mc - 1] == 1 ? 0 : (int?) null; // east 
                                q[state, 4] = null; // west
                                break;
                            default:
                                q[state, 3] = grid[mr, mc - 1] == 1 ? 0 : (int?) null; // east
                                q[state, 4] = grid[mr, mc + 1] == 1 ? 0 : (int?) null; // west
                                break;
                        }

                        // mouse y
                        switch (mr) {
                            case 0:
                                q[state, 0] = null; // north
                                q[state, 1] = grid[mr + 1, mc] == 1 ? 0 : (int?) null; // south
                                break;
                            case 10:
                                q[state, 0] = grid[mr - 1, mc] == 1 ? 0 : (int?) null; // north
                                q[state, 1] = null; // south
                                break;
                            default:
                                q[state, 0] = grid[mr - 1, mc] == 1 ? 0 : (int?) null; // north
                                q[state, 1] = grid[mr + 1, mc] == 1 ? 0 : (int?) null; // south
                                break;
                        }

                        // for any valid grid square, not moving will always be valid
                        q[state, 4] = 0; // none
                        state++;
                    }
                }
            }
        }
    }

    private void Mouse() {
        var random = new Random();
        var epsilon = (float) random.NextDouble();
    }
}
