import React from "react";
import { useNavigate }from "react-router-dom";

import Header from "../components/Header";

import ChatBot from "../components/chatbot/ChatBot";
import ChatBotToggler from '../components/chatbot/ChatBotToggler';

function About() {
    const navigate = useNavigate();

    function handleNavigate(event: React.MouseEvent<HTMLButtonElement>) {
        const {name} = event.target as HTMLButtonElement
        navigate(name);
    }

    function handleToggle() {
        const body = document.body.classList.toggle('show-chatbot');
        console.log(body)
    }

    return(
    <div>
        <Header handleNavigate={handleNavigate}/>
        <div className="intro">
            <h1 className="intro__title">Tinder for pets®</h1>
            <h3 className="intro__description">We believe every pet deserves a buddy!</h3>
        </div>
        <ChatBot/>
        <ChatBotToggler handleToogle={handleToggle}/>
    </div>
    );
}

export default About;