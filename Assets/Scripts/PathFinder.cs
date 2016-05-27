using UnityEngine;
using System.Collections;

public class PathFinder : MonoBehaviour {

	public static Vector2 MAX_VECTOR2 = new Vector2 (float.MaxValue, float.MaxValue);

	private GameObject floor;
	private Floor floorScript;

	private GameObject trailMaker;

	private Vector2 startPos;

	private Move moveTree;

	public GameObject trailPrefab;

    private Color pathfinderBlockColor;

    private int[,] pathCounter;

    //Very dirty way of stopping the path from going infinite in cycles.
    private int maxPathLength = 100;
    private int currentPathLength = 0;

    // Use this for initialization
    void Start () {
        pathfinderBlockColor = Color.white;
        pathfinderBlockColor.a = 0.05f;
        floor = GameObject.Find ("Floor");
		floorScript = floor.GetComponent<Floor> ();
		trailMaker = GameObject.Find ("TrailMaker");
		startPos = new Vector2 (floorScript.startX, floorScript.startZ);
        pathCounter = new int[floorScript.getFloor().GetLength(0), floorScript.getFloor().GetLength(1)];
	}
	
	// Update is called once per frame
	void Update () {
		//rebuildTree ();
	}

	public void rebuildTree() {
        currentPathLength = 0;
        moveTree = buildTree (startPos);
		buildTrail ();
	}

	private Move buildTree(Vector2 position) {
		transform.position = floorScript.getVectorAtCoords ((int)position.x, (int)position.y);
		Move toReturn = new Move (position);

        Vector2 left = floorScript.getCoordAtVector (transform.position + transform.right*-1);
		Vector2 right = floorScript.getCoordAtVector (transform.position + transform.right);
		Vector2 forward = floorScript.getCoordAtVector (transform.position + transform.forward);

        currentPathLength++;
        if (currentPathLength >= maxPathLength) return toReturn;

        if (floorScript.positionOnBlock((int)forward.x, (int)forward.y)) {
			//can go forward, therefore go forward
			//print("Adding forward child");
			transform.LookAt (floorScript.getVectorAtCoords ((int)forward.x, (int)forward.y));
			toReturn.setForward (buildTree (forward));
			toReturn.setChildNum (1);
		} 
		else {
			if (floorScript.positionOnBlock ((int)left.x, (int)left.y)) {
				if (floorScript.positionOnBlock ((int)right.x, (int)right.y)) {
					//can go both left and right, choose random
					//print("Adding left and right children");
					transform.LookAt (floorScript.getVectorAtCoords ((int)left.x, (int)left.y));
					toReturn.setLeft (buildTree (left));

					transform.LookAt (floorScript.getVectorAtCoords ((int)right.x, (int)right.y));
					toReturn.setRight (buildTree (right));

					toReturn.setChildNum (2);
				} else {
					//can only go left
					//print("Adding left child");
					transform.LookAt (floorScript.getVectorAtCoords ((int)left.x, (int)left.y));
					toReturn.setLeft (buildTree (left));
					toReturn.setChildNum (1);
				}
			} else if (floorScript.positionOnBlock ((int)right.x, (int)right.y)) {
				//can only go right
				//print("Adding right child");
				transform.LookAt (floorScript.getVectorAtCoords ((int)right.x, (int)right.y));
				toReturn.setRight (buildTree (right));
				toReturn.setChildNum (1);
			} else {
				//walk off edge
				//print("Adding no children");
				toReturn.setChildNum(0);
			}
		}

		return toReturn;

	}

	public void buildTrail() {
		Move currentNode = moveTree;
		bool atEnd = false;
        currentPathLength = 0;

		while (!atEnd) {
			
			Vector3 position = floorScript.getVectorAtCoords ((int)currentNode.getPosition ().x, (int)currentNode.getPosition ().y);
			GameObject block = (GameObject)Instantiate (trailPrefab, position, Quaternion.identity);
            block.GetComponent<Renderer>().material.color = pathfinderBlockColor;
            currentPathLength++;
            if (currentPathLength >= maxPathLength) atEnd=true;

			if (currentNode.getChildNum () == 1) {
				//if only one possible child continue drawing the trail.
				currentNode = currentNode.findOnlyChild ();
			} else if (currentNode.getChildNum () == 2) {
				//If at a fork, show left and right children, then stop the trail.
				Move leftChild = currentNode.getLeft ();
				Move rightChild = currentNode.getRight ();

				Vector3 leftPos = floorScript.getVectorAtCoords ((int)leftChild.getPosition ().x, (int)leftChild.getPosition ().y);
				Vector3 rightPos = floorScript.getVectorAtCoords ((int)rightChild.getPosition ().x, (int)rightChild.getPosition ().y);

                block = (GameObject)Instantiate (trailPrefab, leftPos, Quaternion.identity);
                block.GetComponent<Renderer>().material.color = pathfinderBlockColor;
                block = (GameObject)Instantiate (trailPrefab, rightPos, Quaternion.identity);
                block.GetComponent<Renderer>().material.color = pathfinderBlockColor;
                atEnd = true;
			} else {
				//If no children stop the trail.
				atEnd = true;
			}
		}
        transform.LookAt(floorScript.getVectorAtCoords(1, 0)); //Need to reset the look direction. Otherwise if you end facing left the dodo doesn't know what to do.
    }

    /* This stuff doesn't work.
	public Vector2 findNextMove(Vector2 currentPosition) {
        //Move currentNode = buildTree(currentPosition);
        resetPathCounter();
        Move currentNode = traverse (moveTree, currentPosition);

        if (currentNode == null) {
			return MAX_VECTOR2;
		}

		if (currentNode.getChildNum () == 1) {
			return currentNode.findOnlyChild ().getPosition ();
		} else if (currentNode.getChildNum () == 2) {
			int randBlock = Random.Range (0, 2);
			if (randBlock == 0) {
				return currentNode.getLeft ().getPosition ();
			} else {
				return currentNode.getRight ().getPosition (); 
			}
		} else {
			return currentNode.getPosition ();
		}

	}

    private void resetPathCounter()
    {
        pathCounter = new int[floorScript.getFloor().GetLength(0), floorScript.getFloor().GetLength(1)];
    }

	public Move traverse(Move currentNode,  Vector2 searchPosition) {
        pathCounter[(int)currentNode.getPosition().x, (int)currentNode.getPosition().y]++;
        if (pathCounter[(int)currentNode.getPosition().x,(int)currentNode.getPosition().y]>=5)
        {
            return null;
        }
		if (currentNode.getPosition() == searchPosition) {
			return currentNode;
		} else if (currentNode.getChildNum() == 1) {
			return traverse (currentNode.findOnlyChild (), searchPosition);
		} else if (currentNode.getChildNum() == 2) {
			Move left = traverse (currentNode.getLeft (), searchPosition);
			Move right = traverse (currentNode.getRight (), searchPosition);
			if (left != null) {
				return left;
			}
			return right;
		} else {
			return null;
		}
	}
    */

	public void printTree(Move currentNode) {
		print (currentNode);

		if (currentNode.getLeft () != null)
			printTree (currentNode.getLeft ());

		if (currentNode.getRight () != null)
			printTree (currentNode.getRight ());

		if (currentNode.getForward () != null)
			printTree (currentNode.getForward ());
	}


	//Inner class representing a move in the tree
	public class Move {

		private Vector2 position;

		private Move left;
		private Move right;
		private Move forward;

		private int childNum;

		public Move(Vector2 position) {
			this.position = position;
		}

		public Move findOnlyChild() {
			if (childNum != 1) { return null;}

			if (this.left != null) {
				return this.left;
			} else if (this.right != null) {
				return this.right;
			} else {
				return this.forward;
			}
		}

		public override string ToString() {
			return position.ToString ();
		}


		//GETTERS
		public Vector2 getPosition() {
			return position;
		}
		public int getChildNum(){
			return childNum;
		}
		public Move getLeft() {
			return left;
		}
		public Move getRight() {
			return right;
		}
		public Move getForward() {
			return forward;
		}


		//SETTERS
		public void setChildNum(int childNum) {
			this.childNum = childNum;
		}
		public void setLeft(Move left) { 
			this.left = left; 
		}
		public void setRight(Move right) {
			this.right = right;
		}
		public void setForward(Move forward) {
			this.forward = forward;
		}
	}
}
