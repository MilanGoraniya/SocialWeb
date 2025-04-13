const API_KEY = "YOUR_API"; // Replace with your actual Gemini API key
const API_URL = `https://generativelanguage.googleapis.com/v1/models/gemini-1.5-flash:generateContent?key=${API_KEY}`;

async function sendMessage() {
    const userInput = document.getElementById("userInput").value;
    if (!userInput.trim()) return;

    appendMessage("user", userInput);
    document.getElementById("userInput").value = ""; // Clear input

    const requestBody = {
        contents: [{ parts: [{ text: userInput }] }]
    };

    try {
        const response = await fetch(API_URL, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(requestBody)
        });

        if (!response.ok) {
            throw new Error(`API error: ${response.status} - ${await response.text()}`);
        }

        const result = await response.json();
        const aiReply = result.candidates[0].content.parts[0].text || "Sorry, I couldn't understand that.";

        appendMessage("ai", aiReply);
    } catch (error) {
        console.error("Error fetching AI response:", error);
        appendMessage("ai", "Error fetching response. Please check API key.");
    }
}

function appendMessage(sender, message) {
    const chatBox = document.getElementById("chatBox");
    const messageElement = document.createElement("div");
    messageElement.classList.add("message", sender);
    messageElement.innerText = message;
    chatBox.appendChild(messageElement);
    chatBox.scrollTop = chatBox.scrollHeight;
}
