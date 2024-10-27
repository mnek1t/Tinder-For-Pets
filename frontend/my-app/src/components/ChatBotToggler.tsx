import '../styles/chatbot.css';
import chatBotlogo from '../assets/chat-dots-svgrepo-com.svg'
import closeIcon from "../assets/close-icon.svg";
interface Chatbot {
    handleToogle : () => void
}
export default function ChatBotToggler({handleToogle} : Chatbot) {
    return(
        <button id="chatbot-toggler" onClick={handleToogle}>
            <span className="material-symbols-rounded">
                <img className='chatbot-logo' src={chatBotlogo} alt="chatBotlogo"></img>
                <p>Message</p>
            </span>
            <span className="material-symbols-rounded">
                <img className='close__image' src={closeIcon} alt="chatBotlogo"></img>
            </span>
        </button>
    );
}