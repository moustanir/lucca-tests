import React from 'react';
import { ChatBoxMessageProps } from '../contracts';

export const ChatBoxMessage: React.FC<ChatBoxMessageProps> = ({ message, id, reader }) => {
    return (
        <div key={id} data-testid="chatBoxMessage-container">
            <div data-testid="chatBoxMessage-author">
                {message.author === reader ? "Me" : message.author}:
            </div>
            <div data-testid="chatBoxMessage-content">
                {message.content}
            </div>
        </div>)
}

export default ChatBoxMessage;
ChatBoxMessage.displayName = 'ChatBoxMessage';