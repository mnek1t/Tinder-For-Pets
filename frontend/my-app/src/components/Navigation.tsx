import '../styles/navigation.css';
import ruleIcon from '../assets/rule-svgrepo-com.svg';
import mainIcon from '../assets/paws_logo.svg'
import { useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

interface NavigationComponentProps {
    profileName: string,
}

export default function Navigation(props: NavigationComponentProps) {
    const location = useLocation();
    const navigate = useNavigate();
    const [navigationLink, setNavigationLink] = useState<string>(location.pathname === "/app/recommendation" ? "/app/profile" : "/app/recommendation");

    function navigateTo() {
        navigate(navigationLink);
    }
    return (
        <>
            <nav className='navigation-component'>
                <button className="rule-communication-page-btn" onClick={navigateTo}>
                    <img src={mainIcon} alt='rule' className='rule-communication-img'></img>
                </button>
                <button className="rule-communication-page-btn">
                    <img src={ruleIcon} alt='rule' className='rule-communication-img'></img>
                </button>
            </nav>
        </>
    );
}
