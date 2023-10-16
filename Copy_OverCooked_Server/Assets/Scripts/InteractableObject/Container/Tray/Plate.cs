using System.Collections.Generic;

public class Plate : Tray
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