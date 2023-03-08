namespace Controller
{
    // w inspektorze klasa, serializable
    [System.Serializable]
    public class ChickenController
    {
        public MoveController moveController;
        
        public ChickenController()
        {
            moveController = new MoveController();
        }

        private void ChickenDeath()
        {
            
        }
    }
}
// metoda onTriggerEnter2D(){} ma byc z coin'em
// zmienna pole na wstawienie clipu i zeby zagral go raz zagrac dzwiek i usunal

// znajdz obiekt z tagiem coincounter i wywolaj metode add coin 