namespace Engine.Graph
{
    public struct RenderContext
    {
        public int VertexArrayObject; 
        public int VertexBufferObject;
        public int Count;
        
        public RenderContext(int vertexArrayObject, int vertexBufferObject, int count)
        {
            VertexArrayObject = vertexArrayObject;
            VertexBufferObject = vertexBufferObject;
            Count = count;
        }
    }
}
