namespace Store.Tests
{
    public class BookTests
    {
        [Fact]
        public void IsIsbn_WhithNull_ReturnFalse()
        {
            bool actual = Book.IsIsbn(null);
            Assert.False(actual);
        }
        [Fact]
        public void IsIsbn_WhithBlank_ReturnFalse()
        {
            bool actual = Book.IsIsbn("   ");
            Assert.False(actual);
        }
        [Fact]
        public void IsIsbn_WhithInvalid_ReturnFalse()
        {
            bool actual = Book.IsIsbn("ISBN 111");
            Assert.False(actual);
        }
        [Fact]
        public void IsIsbn_Whith10_ReturnTrue()
        {
            bool actual = Book.IsIsbn("IsBn 111-0 456-543");
            Assert.True(actual);
        }
    }
}