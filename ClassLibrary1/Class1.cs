namespace ClassLibrary1;
/// <summary>
/// <inheritdoc />
/// </summary>
class C1 : C4
{
    public override void Foo()
    {
    }

    class C2
    {
        class C3;
    }
}

/// <summary>
/// C4 <see cref="Object"/>
/// </summary>
class C4
{
    /// <summary>
    /// Foo
    /// </summary>
    public virtual void Foo()
    {
    }

    /// <summary>
    /// C5
    /// </summary>
    class C5
    {
        /// <summary>
        /// C6
        /// </summary>
        class C6
        {
        }
    }
}
