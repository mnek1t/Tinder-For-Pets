import '../styles/about.css'
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import AuthorCard from '../components/AuthorCard';
import Mykyta from '../assets/mykyta.png'
import Ruslan from '../assets/ruslan.png'
import Mariana from '../assets/mariana.png'
import Footer from "../components/Footer";
function AuthorsPage() {
    const navigate = useNavigate();
    function handleNavigate(event: React.MouseEvent<HTMLButtonElement>) {
        const {name} = event.target as HTMLButtonElement
        navigate(name);
    }
    return(
        <>
            <div className="page-container">
                <Header handleNavigate={handleNavigate}/>
                <main className="authors-container">
                    <AuthorCard name="Ruslan Dzhubuev" description='Frontend, Database' imageSrc={Ruslan}/>
                    <AuthorCard name="Mykyta Medvediev" description='Architecture, Backend, Frontend' imageSrc={Mykyta}/>
                    <AuthorCard name="Mariana Mechyk" description='User Interface, Database' imageSrc={Mariana}/>
                </main>
            </div>
            <Footer/>
        </>
    );
}

export default AuthorsPage;