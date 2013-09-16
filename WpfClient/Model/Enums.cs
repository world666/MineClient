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
        mi2 = 1, mi4, mi6 , mi25, mi26, mi27, mi28, mi29, mi30, mi31, mi188
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
