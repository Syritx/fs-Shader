open System

open OpenTK.Windowing.Common
open OpenTK.Windowing.Desktop
open OpenTK.Graphics.OpenGL
open OpenTK.Mathematics

open Shaders

type Scene(GWS, NWS) =
    inherit GameWindow(GWS, NWS)

    let vertices = [|
        // X    Y     Z     R     G     B
        -0.5f; -0.5f; 0.0f; 1.0f; 0.0f; 0.0f;
         0.5f; -0.5f; 0.0f; 0.0f; 1.0f; 0.0f;
         0.0f;  0.5f; 0.0f; 0.0f; 0.0f; 1.0f
    |]

    let shader = new Shader("src/glsl/vertexShader.glsl", "src/glsl/fragmentShader.glsl")
    let vao = GL.GenVertexArray()
    let vbo = GL.GenBuffer()

    override this.OnRenderFrame(e) =
        base.OnRenderFrame(e)
        GL.Clear(ClearBufferMask.ColorBufferBit)

        shader.Use()
        GL.BindVertexArray(vao)
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3)

        this.SwapBuffers()

    override this.OnLoad() =
        base.OnLoad()

        shader.CreateShader()

        GL.BindVertexArray(vao)
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo)
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof<float>, vertices, BufferUsageHint.StaticDraw)

        GL.BindVertexArray(vao)

        // sizeof(float) in C# is 4, hense the mulitplication of 4
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * 4, 0)
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * 4, 3 * 4)
        GL.EnableVertexAttribArray(0)
        GL.EnableVertexAttribArray(1)

        GL.ClearColor(0.0f ,0.0f ,0.0f ,1.0f)


[<EntryPoint>]
let main argv =

    let NWS = new NativeWindowSettings()

    NWS.Size <- new Vector2i(1200,720)
    NWS.Title <- "Window"
    NWS.StartFocused <- true
    NWS.StartVisible <- true
    NWS.APIVersion <- new Version(3,2)
    NWS.Flags <- ContextFlags.ForwardCompatible
    NWS.Profile <- ContextProfile.Core

    let wind = new Scene(GameWindowSettings.Default, NWS)
    wind.Run()
    0
