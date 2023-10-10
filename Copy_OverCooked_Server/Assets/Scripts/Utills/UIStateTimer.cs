using System.Collections;

public class UIStateTimer
{
    public IEnumerator GetCoroutine()
    {
        return ProgressCoroutine();
    }

    private IEnumerator ProgressCoroutine()
    {
        yield return null;
    }

}