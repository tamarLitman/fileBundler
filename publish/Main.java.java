package Exceptions;

public class Main {

	public static void main(String[] args) {
		try {
			textConvert.convert("12a456*");
		}
		catch(notValidCodeException e) {
			System.out.println("ιετι");
		}
	}
}
