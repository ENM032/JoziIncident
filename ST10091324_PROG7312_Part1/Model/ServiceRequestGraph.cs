using System;
using System.Collections.Generic;

namespace ST10091324_PROG7312_Part1.Model
{
    // YouTube video
    // Titled: Graph Data Structure Implementation | Adjacency List | Adjacency Matrix | C#
    // Uploaded by: TechWebDots
    // Available at: https://www.youtube.com/watch?v=E8rSc58M0g4&list=PLBEm2Vv2nD-MFxyeYuPgPGzkpYJv-nQGI&index=3
    public class ServiceRequestGraph
    {
        private Dictionary<string, List<ServiceRequest>> _graph;

        public ServiceRequestGraph()
        {
            _graph = new Dictionary<string, List<ServiceRequest>>();
        }

        // Add a dependency between two service requests
        public void AddDependency(ServiceRequest from, ServiceRequest to)
        {
            if (!_graph.ContainsKey(from.RequestID))
            {
                _graph[from.RequestID] = new List<ServiceRequest>();
            }

            if (!_graph.ContainsKey(to.RequestID))
            {
                _graph[to.RequestID] = new List<ServiceRequest>();
            }

            _graph[from.RequestID].Add(to);
        }

        // Perform a depth-first search to find dependencies with cycle detection
        public List<ServiceRequest> GetDependencies(ServiceRequest request)
        {
            var visited = new HashSet<string>(); // Tracks all visited nodes
            var recursionStack = new HashSet<string>(); // Tracks nodes in the current recursion path
            var dependencies = new List<ServiceRequest>();

            // Start DFS from the given node
            bool hasCycle = DFSWithCycleDetection(request.RequestID, visited, recursionStack, dependencies);

            if (hasCycle)
            {
                throw new InvalidOperationException("Cycle detected in the dependency graph.");
            }

            return dependencies;
        }

        // Iterative DFS with cycle detection (avoids stack overflow)
        private bool DFSWithCycleDetection(string nodeId, HashSet<string> visited, HashSet<string> recursionStack, List<ServiceRequest> dependencies)
        {
            Stack<string> stack = new Stack<string>();
            stack.Push(nodeId);

            while (stack.Count > 0)
            {
                string currentNodeId = stack.Pop();

                // If the node is currently in the recursion stack, we have detected a cycle
                if (recursionStack.Contains(currentNodeId))
                {
                    return true; // Cycle detected
                }

                // If the node has already been visited, we skip it
                if (visited.Contains(currentNodeId))
                {
                    continue;
                }

                // Mark the node as visited and add it to the recursion stack
                visited.Add(currentNodeId);
                recursionStack.Add(currentNodeId);

                if (_graph.ContainsKey(currentNodeId))
                {
                    // Add all dependencies of the current node to the stack
                    foreach (var dependentRequest in _graph[currentNodeId])
                    {
                        dependencies.Add(dependentRequest); // Add dependent request to the result list
                        stack.Push(dependentRequest.RequestID); // Push the dependent request's ID onto the stack
                    }
                }

                // After processing, remove the current node from the recursion stack
                recursionStack.Remove(currentNodeId);
            }

            return false; // No cycle detected
        }
    }
}
