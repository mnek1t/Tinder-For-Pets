import '../styles/about.css'
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import Footer from '../components/Footer';
import SubscriptionCard from '../components/SubscriptionCard';
import { GENERAL_DESCRIPTION, SUBSCRIPTIONS } from '../utils/TinderConstants'; 
function ProductPage() {
    const navigate = useNavigate();
    function handleNavigate(event: React.MouseEvent<HTMLButtonElement>) {
        const {name} = event.target as HTMLButtonElement
        navigate(name);
    }
    return(
        <>
            <div className="page-container">
                <Header handleNavigate={handleNavigate}/>
                <div className="products-container">
                    <SubscriptionCard name="Basic" benefitsList={SUBSCRIPTIONS.basic.map((benefit, index) => ({id: index, benefitText: benefit}))} price={0.00}/>
                    <SubscriptionCard name="Gold" benefitsList={SUBSCRIPTIONS.gold.map((benefit, index) => ({id: index, benefitText: benefit}))} price={5.00}/>
                    <SubscriptionCard name="Brilliant" benefitsList={SUBSCRIPTIONS.brilliant.map((benefit, index) => ({id: index, benefitText: benefit}))} price={10.00}/>
                </div>
                <div className='description-container'>
                    <article>
                        <h2>What is Tinder For Pets?</h2>
                        <p>{GENERAL_DESCRIPTION}</p>
                    </article>
                </div>
                <Footer/>
            </div>
            
        </>
    );
}

export default ProductPage;