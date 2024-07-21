public class Frame : Bit {
    public char Letter;

    LetterDisplay letterDisplay;
    
    public void Init(int id, BitType type, char letter) {
        base.Init(id, type);

        if (letter != '_' && id != 0) { // frame_ and frame_core
            Letter = letter;
            letterDisplay = GetComponentInChildren<LetterDisplay>();
            letterDisplay.SetLetter(letter);
        }
    }

    public override void Activate() {
        foreach (Bit bit in Children()) {
            if (bit is Frame) continue;
            bit.Activate();
        }
    }

    public override void Deactivate() {
        foreach (Bit bit in Children()) {
            if (bit is Frame) continue;
            bit.Deactivate();
        }
    }
}