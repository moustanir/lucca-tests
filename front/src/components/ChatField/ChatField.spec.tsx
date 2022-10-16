import '@testing-library/jest-dom';
import {render, screen} from '@testing-library/react';
import { ChatBoxesContext } from '../../contexts';
import ChatField from './ChatField';

describe('ChatBox tests suite', () =>{
    it('should render the component',() =>{
        // Given
        const authorName = 'authorName'; 
        
        // When
        render(<ChatField author={authorName}/>);
        const chatBoxContainer = screen.getByTestId('chatfield-container');
        
        // Then 
        expect(chatBoxContainer).toBeInTheDocument();
    });

    it('should render an input',() =>{
        // Given
        const authorName = 'authorName'; 
        
        // When
        render(<ChatField author={authorName}/>);
        const inputFound = screen.getByTestId('chatfield-input') as HTMLInputElement;
        
        // Then 
        expect(inputFound).toBeInTheDocument();
        expect(inputFound.placeholder).not.toBe('');
    });

    it('should render a button',() =>{
        // Given
        const authorName = 'authorName'; 
        
        // When
        render(<ChatField author={authorName}/>);
        const buttonFound = screen.getByTestId('chatfield-button') as HTMLButtonElement;
        
        // Then 
        expect(buttonFound).toBeInTheDocument();
        expect(buttonFound.innerHTML).toBe('Ok');
    });

    it('should call a method',() =>{
        // Given
        const authorName = 'authorName'; 
        const setMessagesMock = jest.fn();
        
        // When
        render(
            <ChatBoxesContext.Provider value={{ messages: [], setMessages: setMessagesMock }}>
                <ChatField author={authorName} />
            </ChatBoxesContext.Provider>);
        const inputFound = screen.getByTestId('chatfield-input') as HTMLInputElement;
        inputFound.value = 'test';
        const buttonFound = screen.getByTestId('chatfield-button') as HTMLButtonElement;
        buttonFound.click();

        // Then 
        expect(setMessagesMock).toBeCalled();
    });
})