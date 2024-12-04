using System;
using System.Collections.Generic;

namespace ST10091324_PROG7312_Part1.Model
{
    // YouTube video
    // Titled: Binary Search Tree implemented in C#
    // Uploaded by: kc70
    // Available at: https://www.youtube.com/watch?v=pN1RWeX47tg
    public class ServiceRequestBST
    {
        private class Node
        {
            public ServiceRequest Data;
            public Node Left;
            public Node Right;

            public Node(ServiceRequest data)
            {
                Data = data;
                Left = Right = null;
            }
        }

        private Node _root;
        private int _size;  // Counter to track the size of the tree

        public ServiceRequestBST()
        {
            _root = null;
            _size = 0;
        }

        // Insert a new service request based on RequestID
        public void Insert(ServiceRequest request)
        {
            _root = Insert(_root, request);
        }

        private Node Insert(Node node, ServiceRequest request)
        {
            if (node == null)
            {
                return new Node(request);
            }

            int comparison = string.Compare(request.RequestID, node.Data.RequestID, StringComparison.Ordinal);

            if (comparison == 0)
            {
                // Update existing node with new request if necessary
                node.Data = request;  // Or return node to avoid duplication
            }
            else if (comparison < 0)
            {
                node.Left = Insert(node.Left, request);
            }
            else if (comparison > 0)
            {
                node.Right = Insert(node.Right, request);
            }

            return node;
        }

        // New method to update the status of a service request by RequestID
        public bool UpdateStatus(string requestId, string newStatus)
        {
            // Search for the request and update the status
            var node = FindNode(_root, requestId);
            if (node != null)
            {
                node.Data.Status = newStatus;
                return true; // Successfully updated
            }
            return false; // Request not found
        }

        // Helper method to find a node by RequestID
        private Node FindNode(Node node, string requestId)
        {
            if (node == null) return null;

            int comparison = string.Compare(requestId, node.Data.RequestID, StringComparison.Ordinal);
            if (comparison == 0) // Found the request
            {
                return node;
            }
            else if (comparison < 0) // Go left
            {
                return FindNode(node.Left, requestId);
            }
            else // Go right
            {
                return FindNode(node.Right, requestId);
            }
        }

        public List<ServiceRequest> SearchAll(string query, Guid userId)
        {
            List<ServiceRequest> results = new List<ServiceRequest>();
            SearchAll(_root, query.ToLower(), results, userId);
            return results;
        }

        private void SearchAll(Node node, string query, List<ServiceRequest> results, Guid userId)
        {
            if (node == null) return;

            // If a match is found, add it to the results list
            if (node.Data.RequestID.ToLower().Contains(query) && node.Data.UserId == userId)
            {
                results.Add(node.Data);
            }

            // Search both subtrees
            SearchAll(node.Left, query, results, userId);
            SearchAll(node.Right, query, results, userId);
        }

        // List all requests in order
        public List<ServiceRequest> InOrderTraversal(Guid userId)
        {
            var result = new List<ServiceRequest>();
            InOrderTraversal(_root, result, userId);
            return result;
        }

        private void InOrderTraversal(Node node, List<ServiceRequest> result, Guid userId)
        {
            if (node != null)
            {
                InOrderTraversal(node.Left, result, userId);

                // Check if the current node's data matches the userId
                if (node.Data.UserId == userId)
                {
                    _size++;
                    result.Add(node.Data);
                }

                //result.Add(node.Data);
                InOrderTraversal(node.Right, result, userId);
            }
        }

        // Method to check if the BST is empty
        public bool IsEmpty()
        {
            return _root == null;
        }

        public int Size()
        {
            return _size;  // Simply return the counter value
        }
    }
}
