public class Frame : Bit {
    public char Letter;

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