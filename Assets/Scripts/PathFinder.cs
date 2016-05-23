using UnityEngine;
using System.Collections;

public class PathFinder : MonoBehaviour {

	public static Vector2 MAX_VECTOR2 = new Vector2 (float.MaxValue, float.MaxValue);

	private GameObject floor;
	private Floor floorScript;

	private Move moveTree;

	// Use this for initialization
	void Start () {
		floor = GameObject.Find ("Floor");
		floorScript = floor.GetComponent<Floor> ();
		//rebuildTree ();
	}
	
	// Update is called once per frame
	void Update () {
		//rebuildTree ();
	}

	public void rebuildTree() {
		moveTree = buildTree (new Vector2 (floorScript.startX, floorScript.startZ));
		print("Tree built");
		printTree (moveTree);
	}

	private Move buildTree(Vector2 position) {
		transform.position = floorScript.getVectorAtCoords ((int)position.x, (int)position.y);
		Move toReturn = new Move (position);


		Vector2 left = floorScript.getCoordAtVector (transform.position + transform.right*-1);
		Vector2 right = floorScript.getCoordAtVector (transform.position + transform.right);
		Vector2 forward = floorScript.getCoordAtVector (transform.position + transform.forward);

		print ("World - Left: " + (transform.position + transform.right)*-1 + " Right: " + (transform.position + transform.right) + " Forward: " + (transform.position + transform.forward));
		print ("Grid - Left: " + left + " Right: " + right + " Forward: " + forward);

		if(floorScript.positionOnBlock((int)forward.x, (int)forward.y)) {
			//can go forward, therefore go forward
			print("Adding forward child");
			toReturn.setForward (buildTree (forward));
			toReturn.setChildNum (1);
		} 
		else {
			if (floorScript.positionOnBlock ((int)left.x, (int)left.y)) {
				if (floorScript.positionOnBlock ((int)right.x, (int)right.y)) {
					//can go both left and right, choose random
					print("Adding left and right children");
					toReturn.setLeft (buildTree (left));
					toReturn.setRight (buildTree (right));
					toReturn.setChildNum (2);
				} else {
					//can only go left
					print("Adding left child");
					toReturn.setLeft (buildTree (left));
					toReturn.setChildNum (1);
				}
			} else if (floorScript.positionOnBlock ((int)right.x, (int)right.y)) {
				//can only go right
				print("Adding right child");
				toReturn.setRight (buildTree (right));
				toReturn.setChildNum (1);
			} else {
				//walk off edge
				print("Adding no children");
				toReturn.setChildNum(0);
			}
		}

		return toReturn;

	}

	public Vector2 findNextMove(Vector2 currentPosition) {
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

	public Move traverse(Move currentNode,  Vector2 searchPosition) {
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
