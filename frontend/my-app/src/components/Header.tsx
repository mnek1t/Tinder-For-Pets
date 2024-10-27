import '../styles/header.css';
import pawLogo from '../assets/paws_logo.svg';

interface HeaderProps {
    handleNavigate: (event: React.MouseEvent<HTMLButtonElement>) => void;
}

const Header: React.FC<HeaderProps> = ({ handleNavigate }) => {
    return(
        <nav className="header">
            <div className='header__label'>
                <img className="header__paws_logo" src={pawLogo} alt='paws_logo'/>
                <h1 className="header__title">Tinder For Pets</h1>
                <div className='header__options'>
                    <ul className='header__options_list'>
                        <li><a href='#Products'>Products</a></li>
                        <li><a href='#About Us'>About Us</a></li>
                        <li><a href='#Authors'>Authors</a></li>
                    </ul>
                </div>
            </div>
            
            <div>
                <button className='header__log_in_btn' name="/login" onClick={(e) => handleNavigate(e)}>Log In</button>
                <button className='header__create_acc_btn' name="/register" onClick={(e) => handleNavigate(e)}>Create Account</button>
            </div>
        </nav>
    );
}

export default Header;