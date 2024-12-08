import '../styles/navigation.css';
import ruleIcon from '../assets/rule-svgrepo-com.svg';
import mainIcon from '../assets/paws_logo.svg'
import { useState } from 'react';
import { useLocation } from 'react-router-dom';

interface NavigationComponentProps {
    profileName: string,
    image: string
}

export default function Navigation(props: NavigationComponentProps) {
    const location = useLocation();

    // Compare location.pathname to determine the navigation link
    const [navigationLink, setNavigationLink] = useState<string>(location.pathname === "/app/recommendation" ? "/app/profile" : "/app/recommendation");

    console.log(location.pathname); // Debug log to check the current path

    return (
        <>
            <nav className='navigation-component'>
                <a className="navigaton-component__link" href={navigationLink} title={location.pathname === "/app/recommendation" ? 'My profile' : "Recommendations"}>
                    <div className="navigation-image-wrapper">
                        <div className="navigation-image" aria-hidden="true"></div>
                    </div>
                    {location.pathname === "/app/recommendation" && <h2 className='navigation-profile-name-wrapper'>
                        <span className='navigation-profile-name'>{props.profileName}</span>
                    </h2>}
                </a>
                <button className="rule-communication-page-btn">
                    <img src={ruleIcon} alt='rule' className='rule-communication-img'></img>
                </button>
            </nav>
        </>
    );
}
