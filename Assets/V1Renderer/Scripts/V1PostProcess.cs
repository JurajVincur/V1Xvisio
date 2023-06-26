using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1PostProcess : MonoBehaviour
{
    public MeshFilter leftQuad;
    public MeshFilter rightQuad;
    public bool flipY = true;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Vector2Int screenSize = new Vector2Int(source.width, source.height);
        RenderTexture tempHalf = RenderTexture.GetTemporary(screenSize.x / 2, screenSize.y);
        RenderTexture tempFull = RenderTexture.GetTemporary(screenSize.x, screenSize.y);

        Blit(tempHalf, leftQuad.GetComponent<Renderer>().material, leftQuad.mesh);
        Graphics.CopyTexture(tempHalf, 0, 0, 0, 0, tempHalf.width, tempHalf.height, tempFull, 0, 0, 0, 0);

        Blit(tempHalf, rightQuad.GetComponent<Renderer>().material, rightQuad.mesh);
        Graphics.CopyTexture(tempHalf, 0, 0, 0, 0, tempHalf.width, tempHalf.height, tempFull, 0, 0, tempHalf.width, 0);

        Vector2 blitScale = flipY ? new Vector2(1, -1) : Vector2.one;
        Vector2 blitOffset = flipY ? new Vector2(0, 1) : Vector2.zero;
        Graphics.Blit(tempFull, destination, blitScale, blitOffset);

        RenderTexture.ReleaseTemporary(tempHalf);
        RenderTexture.ReleaseTemporary(tempFull);
    }

    //https://github.com/zalo/MathUtilities/blob/master/Assets/DistortionInversion/DistortionInverter.cs
    private void Blit(RenderTexture destination, Material mat, Mesh mesh)
    {
        // Create an orthographic matrix (for 2D rendering)
        Matrix4x4 projectionMatrix = Matrix4x4.Ortho(-0.5f, 0.5f, -0.5f, 0.5f, -1f, 1f);
        projectionMatrix *= Camera.current.worldToCameraMatrix.inverse;
        RenderTexture prevRT = RenderTexture.active; // Remember the current RenderTexture
        RenderTexture.active = destination;     // Set our own RenderTexture as "active".
        mat.SetPass(0);                       // Set material as "active". Without this, Unity editor will freeze.
        GL.PushMatrix();
        GL.LoadProjectionMatrix(projectionMatrix);   // Push the projection matrix
        GL.Clear(true, true, Color.black);           // Clear the texture
        Graphics.DrawMeshNow(mesh, Matrix4x4.identity); // Draw the mesh!
        GL.PopMatrix();                              // Pop the projection matrix to set it back to the previous one
        RenderTexture.active = prevRT;               // Re-set the RenderTexture to the last used one
    }
}
