using System;
using System.Collections.Generic;

public class Node<T>
{
    private Node<T> parent;
    private List<Node<T>> children = new List<Node<T>>();
    private T value; //Sudoku Value // Do we need this?
    private string path;
    private int depth = 0;

    public Node(T value) {
        this.value = value;
        path = "";
    }

    public T getValue() {
        return value;
    }

    public int getDepth() {
        return depth;
    }

    public string getPath() {
        return path;
    }

    private Node(T value, string path, int depth) {
        this.value = value;
        this.path = path;
        this.depth = depth;
    }

    public Node<T> getParent() {
        return parent;
    }

    public Node<T> getChild(int index) {
        return children[index];
    }

    public Node<T> getChild(string path) {
        if (String.IsNullOrEmpty(path)) return this;
        var index = int.Parse(path.Substring(0, 1));
        return getChild(index).getChild(path.Substring(1));
    }

    public Node<T> createChild(T value) {
        var node = new Node<T>(value, path += children.Count + 1, ++depth);
        children.Add(node);
        return node;
    }
}
