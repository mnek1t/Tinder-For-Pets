import '../styles/header.css';
import pawLogo from '../assets/paws_logo.svg';

const EmptyHeader: React.FC = () => {
    return(
        <nav className="header">
            <div className='header__label'>
                <img className="header__paws_logo" src={pawLogo} alt='paws_logo'/>
                <h1 className="header__title">Tinder For Pets</h1>
            </div>
        </nav>
    );
}

export default EmptyHeader;