using System.Collections.Generic;

public class FryingPan : Tray
{
    protected override bool IsValidObject(List<EObjectSerialCode> serialObjects)
    {
        if (base.IsValidObject(serialObjects))
        {
            return true;
        }
        return false;
    }
}