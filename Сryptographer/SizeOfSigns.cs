namespace Сryptographer
{
    /// <summary>
    /// Collection of enums: 
    /// 
    /// amount of part in array f bytes with encrypted signs  
    /// </summary>
    public enum SizeOfSigns : int
    {
        signRC4=50, 
        signAES=66
    }

    /// <summary>
    /// options in main menu 
    /// </summary>
    public enum OptionMode : int
    {
        Encryption = 0, 
        Decryption,
        Exit
    }

    /// <summary>
    /// options in menu of en-/decryption 
    /// </summary>
    public enum OptionAlgorithm : int
    {
        RC4=0,
        AES, 
        Return,
        Exit
    }
}