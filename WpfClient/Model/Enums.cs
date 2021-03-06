namespace WpfClient.Model
{
    public enum DoorType
    {
        mb100 = 1, mb101 = 2, mb105 = 3,
        mb106 = 4, mb102 = 5, mb103 = 6,
        mb104 = 7, mb114 = 8, mb115 = 9,
    }

    public enum AnalogSignalType
    {
        mi2 = 1, mi4, mi6 , mi25, mi26, mi27, mi28, mi29, mi30, mi31, mi44, mi45, mi46, mi47, mi48, mi49, mi50, mi51, mi52, mi53,
        mi55, mi56, mi57, mi58, mi59, mi60, mi61, mi62, mi63, mi64, mi65, mi66, mi67, mi68, mi69, mi70, mi71, mi188
    }

    public enum FanEnum
    {
        mb11, mb12, fn
    }
    public enum StateEnum
    {
        None, Ok, Warning, Dangerous
    }
    public enum RemouteFanState //enum for remoute control
    {
        Init = 0, OnFan1, OnFan2, Off
    }
}
