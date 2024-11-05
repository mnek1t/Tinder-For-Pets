import React, {useState} from 'react';
import '../../styles/chatbot.css';
import chatBotlogo from '../../assets/chat-dots-svgrepo-com.svg'
import robotlogo from '../../assets/robot-svgrepo-com.svg'
import arrowUp from '../../assets/up-arrow-svgrepo-com.svg'

export default function ChatBot(){
    const [message, setMessage] = useState("");
    function handleChange(event : React.ChangeEvent<HTMLTextAreaElement>) {
        setMessage(event.target.value);
        console.log(message)
    }

    function sendMessage(event : React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
    }
    
    return(
        <div className="chatbot-popup">
            <div className='chat-header'>
                <div className='header-info'>
                    <img className='chatbot-logo' src={chatBotlogo} alt="chatBotlogo"></img>
                    <h2 className="logo-text">Chatbot</h2>
                </div>
            </div>
            <div className="chat-body">
                <div className="message bot-message">
                    <img className="bot-avatar" src={robotlogo} alt="robot logo"></img>
                    <div className="message-text"> Hey there 👋 <br /> How can I help you today?</div>
                </div>
                <div className="chat-footer">
                    <form action="#" className="chat-form" onSubmit={(e) => sendMessage(e)}>
                        <textarea placeholder="Message..." className="message-input" value={message} required onChange={(e) => handleChange(e)}></textarea>
                        <div className="chat-controls">
                            <button type="submit" id="send-message" className="material-symbols-rounded">
                                <img className='arrow__image' src={arrowUp} alt="arrowup"></img>
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
} 